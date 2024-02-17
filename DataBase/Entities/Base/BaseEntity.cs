namespace DataBase.Entities.Base
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsValid { get; set; } = true;
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
