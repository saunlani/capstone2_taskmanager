using System;
namespace capstone2_taskmanager
{
    // class for the Task Manager.
    public class Task
    {
        public string MemberName { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Done { get; set; }

        public Task(string MemberName , 
                    string Description, 
                    DateTime DueDate, 
                    string Done = null)
        {
            this.MemberName = MemberName;
            this.Description = Description;
            this.DueDate = DueDate;
            this.Done = Done;
        }
    }
}
