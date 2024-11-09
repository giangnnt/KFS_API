using KFS.src.Application.Core.Jwt;

namespace KFS.src.Application.Service
{
    public interface IOwnerService
    {
        bool CheckEntityOwner(HttpContext context, Guid userId);
    }
    public class OwnerService : IOwnerService
    {
        public bool CheckEntityOwner(HttpContext context, Guid userId)
        {
            //get payload
            var payload = context.Items["payload"] as Payload;
            //check payload
            if (payload == null)
            {
                Console.WriteLine("Payload is required");
                return false;
            }
            if (payload.RoleId != 1 && payload.RoleId != 2)
            {
                //check own
                if (userId != payload.UserId)
                {
                    Console.WriteLine("Unauthorized");
                    return false;
                }
            }
            return true;
        }
    }
}