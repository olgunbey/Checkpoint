namespace Checkpoint.IdentityServer.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }
    }
}


//Efe Trendyol TeamLead (write,delete,read) 
//Ece Trendyol Admin (write,delete,read)
//Murat Trendyol JrDeveloper (read)
//Arda Trendyol JrDeveloper (write,read)




