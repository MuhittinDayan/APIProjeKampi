using System.ComponentModel.DataAnnotations;

namespace APIProjeKampi_WepApi.Entities
{
    public class Chef
    {
        [Key]
        public int ChefId { get; set; }
        public string NameSurname { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public List<EmployeeTaskChef> EmployeeTaskChefs { get; set; }
    }
}
