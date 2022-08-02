using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoalesceSample.Data.Identity;
public class JwtConfiguration
{
    public string SigningKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpirationInMinutes { get; set; } = 1440;
}
