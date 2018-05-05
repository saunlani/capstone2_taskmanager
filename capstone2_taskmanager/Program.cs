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
     * 
     * TODO:
     * fix datelist bug, it's wiping the original list.
     * fix delete bug
     */

    class Program
    {
        //declares a public list named Tasks using the Task Class type.
        public static List<Task> Tasks = new List<Task>();

        static void Main(string[] args)
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

            Console.WriteLine("Welcome to the Task Manager!");

            bool RunProgram = true;
            while (RunProgram)
            {
                Console.WriteLine("Type \"list\" to list the tasks in due date order.");
                Console.WriteLine("Type \"add\" to add a task.");
                Console.WriteLine("Type \"edit\" to edit a task");
                Console.WriteLine("Type \"delete\" to delete a task.");
                Console.WriteLine("Type \"mark complete\" to mark a task complete.");
                Console.WriteLine("Type \"find\" to display tasks that belong to one person.");
                Console.WriteLine("Type \"datelist\" to display tasks that are due before a specified date.");
                Console.WriteLine("Type \"quit\" to quit the program.");

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
                            Console.WriteLine("Bye!");
                            askingquit = false;
                            RunProgram = false;
                        }
                        else if (quitresponse == "n")
                        { askingquit = false; }
                    }
                }
            }
        }

        static void ListTasks()
        {

            //var pull = from task in Tasks
            // orderby task.DueDate
            //select task;
            Console.WriteLine("#\tDone?\tDue Date\tTeam Member\tTask Description");
            Tasks.Sort((a, b) => a.DueDate.CompareTo(b.DueDate));
            foreach (var item in Tasks)
            {
                var shortdate = item.DueDate.ToString("MM/dd/yyyy");
                int theIndex = Tasks.FindIndex(x => x.MemberName.ToLower() == item.MemberName.ToLower());
                Console.Write($"{theIndex + 1}\t{item.Done}\t{shortdate}\t{item.MemberName}\t\t{item.Description}");
                Console.WriteLine();
            }
            Console.WriteLine();
        }

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
                    Console.WriteLine("What is the Team Member's Name?");
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
                    Console.WriteLine("What is the task description?");
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
                    Console.WriteLine("Enter the due date (MM/DD/YYYY):");
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
                            Console.Write("Not a valid date format\n\n");
                            continue;
                        }
                    }
                }

            }

        }

        /* marking complete */
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

                        if (numbersuccess == false || String.IsNullOrEmpty(userinput) || userinput == "0")
                        {
                            Console.WriteLine("Invalid Input.");
                            continue;
                        }

                        else if (userinput.ToLower() == "cancel")
                        {
                            askfortasknum = false;
                            MarkingComplete = false;
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

        /* finding a name and returning their tasks */
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

        /* deleting a specific task by task number (sort before returning list) */

        static void Deleter()
        {
            bool RetrievingName = true;
            while (RetrievingName == true)
            {
                {
                    bool askforname = true;
                    while (askforname)
                    {
                        Console.WriteLine("Please enter the task description to delete or cancel.");
                        string description = Console.ReadLine().ToLower();

                        var descrip = Tasks.Where(x => x.Description.ToLower() == description.ToLower());

                        if (Tasks.Exists(x => x.Description.ToLower() == description.ToLower()))
                        {
                            foreach (var item in descrip)
                            {

                                Console.WriteLine($"Task Description: { item.Description}");
                                Console.WriteLine($"Due Date: { item.DueDate}");
                                Console.WriteLine($"Complete: { item.Done}");
                                Console.WriteLine("");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{description} doesn't exist.");
                        }
                    }
                }
            }
        }

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
                        Console.WriteLine("Please enter a task number to mark edit or type cancel to return to the main menu.");
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

                        int.TryParse(userinput, out int tasknum);
                        if (tasknum <= 0 || tasknum > Tasks.Count)
                        {
                            Console.WriteLine($"Not within range, there are a total of {Tasks.Count} tasks currently in the Task Manager.");
                            continue;
                        }
                        tasknum = tasknum - 1;
                        Console.WriteLine("Due Date\tTeam Member\tTask Description");
                        Console.WriteLine($"{Tasks[tasknum].DueDate.ToShortDateString()}\t\t{Tasks[tasknum].MemberName}\t\t{Tasks[tasknum].Description}");

                        bool makingchoice = true;
                        while (makingchoice)
                        {
                            Console.WriteLine("Type Name, Description, Date:");
                            string userreponse = Console.ReadLine().ToLower();

                            if (userreponse != "name"
                                && userreponse != "description"
                                && userreponse != "date")
                            {
                                Console.WriteLine("Invalid input.");
                                continue;
                            }
                            else if (userreponse == "name")
                            {
                                Console.WriteLine("Enter new member name for task:  ");
                                Tasks[tasknum].MemberName = Console.ReadLine();
                                makingchoice = false;
                                askfortasknum = false;
                            }
                            else if (userreponse == "description")
                            {
                                Console.WriteLine("Enter new task description:  ");
                                Tasks[tasknum].Description = Console.ReadLine();
                                makingchoice = false;
                                askfortasknum = false;
                            }
                            else if (userreponse == "date")
                            {
                                bool AddingDate = true;
                                while (AddingDate)
                                {
                                    DateTime adate;
                                    Console.WriteLine("Enter the new task due date (MM/DD/YYYY):");
                                    string dateinput = Console.ReadLine();

                                    if (String.IsNullOrWhiteSpace(dateinput))
                                    {
                                        Console.WriteLine("Nothing entered.");
                                        continue;
                                    }

                                    if (DateTime.TryParseExact(dateinput, "MM/dd/yyyy", CultureInfo.InvariantCulture,
                                                                    DateTimeStyles.None, out adate))
                                    {
                                        Tasks[tasknum].DueDate = adate;
                                        Console.WriteLine("Task has been edited.");
                                        AddingDate = false;
                                    }
                                    else
                                    {
                                        Console.Write("Not a valid date format (MM\\DD\\YYY) \n\n");
                                        continue;
                                    }
                                }
                                makingchoice = false;
                                askfortasknum = false;
                            }
                            EditingTask = false;
                        }
                    }
                }
            }
        }
        static void DateList()
        {
            Tasks.Sort((a, b) => a.DueDate.CompareTo(b.DueDate));
            bool gettingtsbefored = true;
            while (gettingtsbefored)
            {
                Console.WriteLine("Enter a date:");
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
                        Tasks = Tasks.Where(x => x.DueDate < duedate || x.DueDate == duedate).ToList();
                        foreach (var item in Tasks)
                        {
                            int theIndex = Tasks.FindIndex(x => x.MemberName.ToLower() == item.MemberName.ToLower());
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
    }
}