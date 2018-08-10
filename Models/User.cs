using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WeddingPlanner.Models
{
    public class User
    {
        public int UserId {get;set;}

        public string FirstName {get;set;}

        public string LastName {get;set;}

        public string Email {get;set;}

        public string Password {get;set;}


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt {get;set;}
        

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt {get;set;}

        public int WeddingId {get;set;}

        public Wedding Wedding {get;set;}

        public List<WeddingAttendance> Attending {get;set;}

        public User()
        {
            Attending = new List<WeddingAttendance>();
        }
    }
}