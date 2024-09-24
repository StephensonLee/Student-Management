using System.ComponentModel.DataAnnotations.Schema;

namespace Student_Management.Models.Domain.Base
{
    public class EntityBase
    {
        protected EntityBase()
        {
            State = State.Unchanged;
        }
        public int Id { get; set; }

        [NotMapped]
        public State State { get; set; }
    }
}
