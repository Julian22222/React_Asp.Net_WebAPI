using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.Service
{
    public class TokenService : ITokenService  //Inherit from ITokkenService (Interface)
    {

        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;  //See line 23


        //constructor
        public TokenService(IConfiguration config)  //IConfiguration needs to get access to 
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));  //we use Encoding because we have to turn ["JWT:SigningKey"] to--> Bites, it won't accept regular string, SigningKey --> must be hidden and Secured from others
            //JWT totally depends from SigningKey
            //SymmetricSecurityKey - is what is going to be used to inscript our SigningKey(password) in unique way that is only specific for our server, so that people can't mess with actual integrity of the token
        }




        public string CreateToken(AppUser user){
            
            var claims = new List<Claim>(){   //set the properties that we can use to identify the User and express what the User can and can't do withing your app. Very similar to the Role( in Data/ApplicationDBContext.cs file)--> but more flexible

                //Here we indicate the properties that will be in Claims, that we can use any time throughout the app as long as the User is LogedIn
                //Email and GivenName properties that Claims uses, we assign Email == user.Email, GivenName == user.UserName
                new Claim(JwtRegisteredClaimNames.Email, user.Email),  //we use JwtRegisteredClaimNames and user because it is Standard of JWT
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName) //GivenName is same as UserName
            };

            //creating Signing credentials - it means what type of inscription do we want 
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); //_key <-- from appsettings.json, SecurityAlgorithms.HmacSha512Signature - is form of inscription that we want to use
        
            //create token as an object representation
            var tokenDescriptor = new SecurityTokenDescriptor(){
                Subject = new ClaimsIdentity(claims),  //ClaimsIdentity is a valet
                Expires = DateTime.Now.AddDays(7),  //JWT has expiration date, we don't want token to have too much life time on, in case if someone will get the access to it , if someone steals this token. We don't want this token last forever
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"], //from appsettings.json, needed this when deploy our App in the Web, Issuer is a server
                Audience = _config["JWT:Audience"], //needed this when deploy our App in the Web, Audience is the Users , who use our App
            };

            //This Method creates actual token for us
            var tokenHandler = new JwtSecurityTokenHandler();

            //This method creates an object representation of the token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //we don't want the token in form of in actual object, we want to return it in form of string
            return tokenHandler.WriteToken(token); //return token in form of string

        }
    }
}