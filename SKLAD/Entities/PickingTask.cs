using SKLAD.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKLAD.Entities
{
    public class PickingTaskEntity
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string AssignedTo { get; set; } // UserId
        public string Status { get; set; } // New, InProgress, Completed
        public List<PickingItem> Items { get; set; } = new();

    }
}
