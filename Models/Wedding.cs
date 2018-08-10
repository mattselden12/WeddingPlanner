using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WeddingPlanner.Models
{
    public class Wedding
    {
        public int WeddingId {get;set;}

        public string Address {get;set;}

        public DateTime Date {get;set;}


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt {get;set;}
        

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt {get;set;}

        public List<User> Wedders {get;set;}

        public List<WeddingAttendance> Attenders {get;set;}

        public Wedding()
        {
            Wedders = new List<User>();
            Attenders = new List<WeddingAttendance>();
        }

    }
}