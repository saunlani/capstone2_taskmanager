using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Globalization;

namespace capstone2_taskmanager
{
    /* task manager:
     * list, add , edit, delete, mark complete, find , datelist, quit
     * allows user to return all tasks before a given date.
     * allows user to return tasks for ONE team member name 
     * fixed all bugs, program accomodates all extended challenges.
     */

    class Program
    {
        //declares a public list named Tasks using the Task Class type.
        public static List<Task> Tasks = new List<Task>();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Task Manager!");
            LoadInitialTasks();
            TheProgram();

        }
        //lists tasks
        static void ListTasks()
        {
            Console.WriteLine("#\tDone?\tDue Date\tTeam Member\tTask Description");
            Tasks.Sort((a, b) => a.DueDate.CompareTo(b.DueDate));
            foreach (var item in Tasks)
            {
                var shortdate = item.DueDate.ToString("MM/dd/yyyy");
                int theIndex = Tasks.FindIndex(x => x.Description.ToLower() == item.Description.ToLower());
                Console.Write($"{theIndex + 1}\t{item.Done}\t{shortdate}\t{item.MemberName}\t\t{item.Description}");
                Console.WriteLine();
            }
        }
        //adds tasks
        static void AddTask()
        {
            string memname = "";
            string descrip = "";
            DateTime duedate = DateTime.Now;

            bool AddingTask = true;
            while (AddingTask)
            {
                bool AddingName = true;
                while (AddingName)
                {
                    Console.Write("What is the Team Member's Name?:  ");
                    string nameinput = Console.ReadLine();

                    bool containsnumbers = nameinput.Any(char.IsDigit);
                    if (containsnumbers == true || String.IsNullOrWhiteSpace(nameinput))
                    {
                        Console.WriteLine("Invalid Name Input.");
                        continue;
                    }
                    else
                    {
                        memname = nameinput;
                        AddingName = false;
                    }
                }

                bool AddingDescrip = true;
                while (AddingDescrip)
                {
                    Console.Write("What is the task description?:  ");
                    string descripinput = Console.ReadLine();

                    if (String.IsNullOrWhiteSpace(descripinput))
                    {
                        Console.WriteLine("Nothing entered.");
                        continue;
                    }
                    else
                    {
                        descrip = descripinput;
                        AddingDescrip = false;
                    }
                }

                bool AddingDate = true;
                while (AddingDate)
                {
                    Console.Write("Enter the due date (MM/DD/YYYY):  ");
                    string dateinput = Console.ReadLine();

                    if (String.IsNullOrWhiteSpace(dateinput))
                    {
                        Console.WriteLine("Nothing entered.");
                        continue;
                    }
                    else
                    {
                        if (DateTime.TryParseExact(dateinput, "MM/dd/yyyy",
                                      CultureInfo.InvariantCulture,
                                                   DateTimeStyles.None, out duedate))
                        {
                            Task newtask = new Task(memname, descrip, duedate, "no");
                            Tasks.Add(newtask);
                            AddingDate = false;
                            AddingTask = false;
                        }
                        else
                        {
                            Console.Write("Not a valid date format\n");
                            continue;
                        }
                    }
                }

            }

        }

        //marks tasks complete
        static void MarkComplete()
        {
            Tasks.Sort((a, b) => a.DueDate.CompareTo(b.DueDate));
            bool MarkingComplete = true;
            while (MarkingComplete == true)
            {
                {
                    bool askfortasknum = true;
                    while (askfortasknum)
                    {
                        Console.WriteLine("Please enter a task number to mark complete or type \"cancel\" to the return to the main menu.");
                        string userinput = Console.ReadLine();
                        bool numbersuccess = userinput.All(char.IsDigit);
                        int.TryParse(userinput, out int tasknum);
                        // check to see if userinput is within the range of the tasks.count

                        if (userinput.ToLower() == "cancel")
                        {
                            askfortasknum = false;
                            MarkingComplete = false;
                        }
                        else if (numbersuccess == false || String.IsNullOrEmpty(userinput) || userinput == "0")
                        {
                            Console.WriteLine("Invalid Input.");
                            continue;

                        }
                        else if (tasknum <= 0 || tasknum > Tasks.Count)
                        {
                            Console.WriteLine($"Not within range, there are a total of {Tasks.Count} tasks currently in the Task Manager.");
                            continue;
                        }
                        tasknum = tasknum - 1;
                        Console.WriteLine("Due Date\tTeam Member\tTask Description");
                        Console.WriteLine($"{Tasks[tasknum].DueDate.ToShortDateString()}\t\t{Tasks[tasknum].MemberName}\t\t{Tasks[tasknum].Description}");

                        bool checkingwuser = true;
                        while (checkingwuser)
                        {
                            Console.WriteLine("Are you sure you want to mark this task as completed? (yes/no)");
                            string markinput = Console.ReadLine().ToLower();

                            if (markinput != "yes" && markinput != "no")
                            {
                                Console.WriteLine("Invalid input.  Please type \"yes\" or \"no\"");
                                continue;
                            }
                            else if (markinput == "yes")
                            {
                                Tasks[tasknum].Done = "yes";
                                Console.WriteLine("Task has been marked as complete.");
                                askfortasknum = false;
                                checkingwuser = false;
                                MarkingComplete = false;
                            }
                            else if (markinput == "no")
                            {
                                askfortasknum = false;
                                checkingwuser = false;
                                MarkingComplete = false;
                            }
                        }


                    }
                }
            }
        }

        //finds tasks for specified name
        static void NameFinder()         {
            bool RetrievingName = true;
            while (RetrievingName == true)
            {
                {
                    bool askforname = true;
                    while (askforname)
                    {
                        Console.WriteLine("Please enter a name to display their tasks or type \"cancel\" to return to the main menu.");
                        string namechoice = Console.ReadLine();

                        var aname = Tasks.Where(x => x.MemberName.ToLower() == namechoice.ToLower());
                        if (Tasks.Exists(x => x.MemberName.ToLower() == namechoice.ToLower()))
                        {
                            foreach (var item in aname)
                            {
                                Console.WriteLine($"Member Name: { item.MemberName}");
                                Console.WriteLine($"Task Description: { item.Description}");
                                Console.WriteLine($"Due Date: { item.DueDate}");
                                Console.WriteLine($"Complete: { item.Done}");
                                Console.WriteLine("");
                            }
                            askforname = false;
                            RetrievingName = false;
                        }
                        else if (namechoice.ToLower() == "cancel")
                        {
                            askforname = false;
                            RetrievingName = false;
                        }
                        else
                        {
                            Console.WriteLine($"{namechoice} doesn't exist.");
                        }
                    }
                }
            }
        }
        //edits a task
        static void EditTask()
        {
            Tasks.Sort((a, b) => a.DueDate.CompareTo(b.DueDate));
            bool EditingTask = true;
            while (EditingTask == true)
            {
                {
                    bool askfortasknum = true;
                    while (askfortasknum)
                    {
                        Console.WriteLine("Please enter a task number to edit or type cancel to return to the main menu.");
                        string userinput = Console.ReadLine();
                        bool numbersuccess = userinput.All(char.IsDigit);
                        // check to see if userinput is within the range of the tasks.count/

                        if (userinput.ToLower() == "cancel")
                        {
                            askfortasknum = false;
                            EditingTask = false;
                        }

                        else if (numbersuccess == false || String.IsNullOrEmpty(userinput) || userinput == "0")
                        {
                            Console.WriteLine("Invalid Input.");
                            continue;

                        }
                        else
                        {
                            int.TryParse(userinput, out int tasknum);
                            if (tasknum <= 0 || tasknum > Tasks.Count)
                            {
                                Console.WriteLine($"Not within range, there are a total of {Tasks.Count} tasks currently in the Task Manager.");
                                continue;
                            }
                            tasknum = tasknum - 1;
                            Console.WriteLine("\nDue Date\tTeam Member\tTask Description");
                            Console.WriteLine($"{Tasks[tasknum].DueDate.ToShortDateString()}\t\t{Tasks[tasknum].MemberName}\t\t{Tasks[tasknum].Description}\n");

                            bool makingchoice = true;
                            while (makingchoice)
                            {
                                Console.Write("Type Name, Description, Date or type \"cancel\" to return to the main menu.\n");
                                string userreponse = Console.ReadLine().ToLower();

                                if (userreponse == "cancel")
                                {
                                    askfortasknum = false;
                                    EditingTask = false;
                                    makingchoice = false;
                                }
                                else if (userreponse != "name"
                                    && userreponse != "description"
                                    && userreponse != "date"
                                    && userreponse != "cancel")
                                {
                                    Console.WriteLine("Invalid input.");
                                    continue;
                                }
                                // edit name
                                else if (userreponse == "name")
                                {
                                    Console.Write("Enter new member name for task:  ");
                                    Tasks[tasknum].MemberName = Console.ReadLine();
                                    makingchoice = false;
                                    Console.WriteLine("Task has been edited.");
                                    askfortasknum = EditingTask = EditAnother();
                                }
                                //edit description
                                else if (userreponse == "description")
                                {
                                    Console.Write("Enter new task description:  ");
                                    Tasks[tasknum].Description = Console.ReadLine();
                                    makingchoice = false;
                                    Console.WriteLine("Task has been edited.");
                                    askfortasknum = makingchoice = EditingTask = EditAnother();
                                }
                                // edit date
                                else if (userreponse == "date")
                                {
                                    bool AddingDate = true;
                                    while (AddingDate)
                                    {
                                        DateTime adate;
                                        Console.Write("Enter the new task due date (MM/DD/YYYY):  ");
                                        string dateinput = Console.ReadLine();

                                        if (String.IsNullOrWhiteSpace(dateinput))
                                        {
                                            Console.WriteLine("Nothing entered.");
                                            continue;
                                        }

                                        else if (DateTime.TryParseExact(dateinput, "MM/dd/yyyy", CultureInfo.InvariantCulture,
                                                                    DateTimeStyles.None, out adate))
                                        {
                                            Tasks[tasknum].DueDate = adate;
                                            Console.WriteLine("Task has been edited.");
                                            AddingDate = false;
                                            makingchoice = false;
                                            askfortasknum  = EditingTask = EditAnother();
                                        }
                                        else
                                        {
                                            Console.Write("Not a valid date format (MM\\DD\\YYY) \n\n");
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        static bool EditAnother()
        {
            Console.WriteLine("Edit another task? (yes/no)");
            string userreply = Console.ReadLine().ToLower();
            bool replying = true;
            bool finalanswer = true;
            while (replying)
            {
                if (userreply != "yes" && userreply != "no")
                {
                    Console.WriteLine("Invlaid input.");
                    continue;
                }
                if (userreply == "yes")
                {
                    replying = false;
                    finalanswer = true;
                    return true;
                }
                else
                {
                    finalanswer = false;
                    return false;
                }
            }
            return (finalanswer);
        }
        //returns a list of tasks up to a specified date, in due date order
        static void DateList()
        {
            Tasks.Sort((a, b) => a.DueDate.CompareTo(b.DueDate));
            bool gettingtsbefored = true;
            while (gettingtsbefored)
            {
                Console.Write("Enter a date:  ");
                string userdate = Console.ReadLine();
                Console.WriteLine();

                if (String.IsNullOrWhiteSpace(userdate))
                {
                    Console.WriteLine("Nothing entered.");
                    continue;
                }
                else
                {
                    DateTime duedate;
                    if (DateTime.TryParseExact(userdate, "MM/dd/yyyy",
                                  CultureInfo.InvariantCulture,
                                               DateTimeStyles.None, out duedate))
                    {
                        Console.WriteLine("#\tDone\tDue Date\tTeam Member\tTask Description");
                        List<Task> dateList = new List<Task>(Tasks);
                        dateList = dateList.Where(x => x.DueDate < duedate || x.DueDate == duedate).ToList();
                        foreach (var item in dateList)
                        {
                            int theIndex = dateList.FindIndex(x => x.MemberName.ToLower() == item.MemberName.ToLower());
                            var shortdate = item.DueDate.ToString("MM/dd/yyyy");
                            Console.Write($"{theIndex + 1}\t{item.Done}\t{shortdate}\t{item.MemberName}\t\t{item.Description}");
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                        gettingtsbefored = false;
                    }
                    else
                    {
                        Console.Write("Not a valid date format (MM\\DD\\YYY) \n\n");
                        continue;
                    }
                }
            }
        }
        //deletes tasks
        static void Deleter()
        {
            Tasks.Sort((a, b) => a.DueDate.CompareTo(b.DueDate));
            bool Deleting = true;
            while (Deleting == true)
            {
                {
                    bool askfortasknum = true;
                    while (askfortasknum)
                    {
                        Console.WriteLine("Please enter a task number to delete or type \"cancel\" to the return to the main menu.");
                        string userinput = Console.ReadLine();
                        bool numbersuccess = userinput.All(char.IsDigit);
                        int.TryParse(userinput, out int tasknum);
                        // check to see if userinput is within the range of the tasks.count

                        if (userinput.ToLower() == "cancel")
                        {
                            askfortasknum = false;
                            Deleting = false;
                        }
                        else if (numbersuccess == false || String.IsNullOrEmpty(userinput) || userinput == "0")
                        {
                            Console.WriteLine("Invalid Input.");
                            continue;
                        }
                        else if (tasknum <= 0 || tasknum > Tasks.Count)
                        {
                            Console.WriteLine($"Not within range, there are a total of {Tasks.Count} tasks currently in the Task Manager.");
                            continue;
                        }
                        else
                        {
                            tasknum = tasknum - 1;
                            Console.WriteLine("\nDone\tDue Date\tTeam Member\tTask Description");
                            Console.WriteLine($"{Tasks[tasknum].Done}\t{Tasks[tasknum].DueDate.ToShortDateString()}\t\t{Tasks[tasknum].MemberName}\t\t{Tasks[tasknum].Description}\n");

                            bool checkingwuser = true;
                            while (checkingwuser)
                            {
                                Console.Write("Are you sure you want to delete this task? (yes/no):  ");
                                string markinput = Console.ReadLine().ToLower();

                                if (markinput != "yes" && markinput != "no")
                                {
                                    Console.WriteLine("Invalid input.  Please type \"yes\" or \"no\"");
                                    continue;
                                }

                                else if (markinput == "yes")
                                {
                                    Tasks.RemoveAt(tasknum);
                                    Console.WriteLine("Task has been deleted.");
                                    askfortasknum = false;
                                    checkingwuser = false;
                                    Deleting = false;
                                }
                                else if (markinput == "no")
                                {
                                    askfortasknum = false;
                                    checkingwuser = false;
                                    Deleting = false;
                                }
                            }


                        }
                    }
                }
            }
        }
        // menu
        static void Menu()
        {
            Console.WriteLine("\nType \"list\" to list the tasks in due date order.");
            Console.WriteLine("Type \"add\" to add a task.");
            Console.WriteLine("Type \"edit\" to edit a task");
            Console.WriteLine("Type \"delete\" to delete a task.");
            Console.WriteLine("Type \"mark complete\" to mark a task complete.");
            Console.WriteLine("Type \"find\" to display tasks that belong to one person.");
            Console.WriteLine("Type \"datelist\" to display tasks that are due before a specified date.");
            Console.WriteLine("Type \"quit\" to quit the program.\n");
        }
        // loads some generic task entries
        static void LoadInitialTasks()
        {
            Task task0 = new Task("Angela", "dishes", new DateTime(2018, 06, 19), "no");
            Task task1 = new Task("Bob", "Bring cookies on Monday.", new DateTime(2018, 06, 20), "yes");
            Task task2 = new Task("Craig", "Carry the crayons.", new DateTime(2018, 06, 21), "yes");
            Task task3 = new Task("Drew", "Draw a mural on the board.", new DateTime(2018, 06, 22), "yes");
            Task task4 = new Task("Edward", "Extend the due dates on all of the tasks", new DateTime(2018, 06, 23), "yes");
            Tasks.Add(task0);
            Tasks.Add(task1);
            Tasks.Add(task2);
            Tasks.Add(task3);
            Tasks.Add(task4);
        }
        //user selects which method(function) to run.
        static void TheProgram()
        {
            bool RunProgram = true;
            while (RunProgram)
            {
                Menu();
                string userresponse = Console.ReadLine().ToLower();

                if (userresponse != "list"
                    && userresponse != "add"
                    && userresponse != "edit"
                    && userresponse != "delete"
                    && userresponse != "mark complete"
                    && userresponse != "find"
                    && userresponse != "datelist"
                    && userresponse != "quit")
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }
                else if (userresponse == "list")
                {
                    Console.WriteLine("");
                    ListTasks();
                }
                else if (userresponse == "add")
                {
                    Console.WriteLine("");
                    AddTask();
                }
                else if (userresponse == "edit")
                {
                    Console.WriteLine("");
                    EditTask();
                }
                else if (userresponse == "find")
                {
                    Console.WriteLine("");
                    NameFinder();
                }
                else if (userresponse == "delete")
                {
                    Console.WriteLine("");
                    Deleter();
                }
                else if (userresponse == "datelist")
                {
                    Console.WriteLine("");
                    DateList();
                }
                else if (userresponse == "mark complete")
                {
                    Console.WriteLine("");
                    MarkComplete();
                }
                else if (userresponse == "quit")
                {
                    bool askingquit = true;
                    while (askingquit)
                    {
                        Console.WriteLine("Are you sure you want to quit? (y/n)");
                        string quitresponse = Console.ReadLine().ToLower();
                        if (quitresponse != "y" && quitresponse != "n")
                        {
                            Console.WriteLine("Invalid input.");
                            continue;
                        }
                        else if (quitresponse == "y")
                        {
                            Console.WriteLine("Goodbye!");
                            askingquit = false;
                            RunProgram = false;
                        }
                        else if (quitresponse == "n")
                        { askingquit = false; }
                    }
                }
            }
        }
    }
}