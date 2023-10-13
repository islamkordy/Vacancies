using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class RefreshToken
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? IsRevoked { get; set; }
        public Guid? UserId { get; set; }
        public bool IsRefreshTokenExpired()
        {
            return DateTime.Now > this.ExpirationDate;
        }
        public void RevokeRefresh()
        {
            this.IsRevoked = !this.IsRevoked;
        }
        public void AddRefresh(string? Token, DateTime? ExpirationDate, Guid? UserId) 
        {
            this.Token = Token;
            this.ExpirationDate = ExpirationDate;
            this.UserId = UserId;
            this.IsRevoked = false;
        }
    }
}
