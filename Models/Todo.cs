using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace authAPI
{
    public class Todo 
    {
        [Key]
        public int TodoId {get; set;}
        public string Description {get; set;}
        public DateTime Deadline {get; set;}

        [ForeignKey("UserId")]
        public int UserId {get; set;}
        public Usuario User {get; set;}
    }
}