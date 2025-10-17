using System;

namespace ViajeHonesto.Filtering
{
    public interface IUserOwned
    {
        Guid UserId { get; set; }
    }
}
