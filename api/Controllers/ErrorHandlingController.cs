using System.IO;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;   //Can you Task with async await , and to use Task
using Microsoft.EntityFrameworkCore;  //to use ToListAsync method, SaveChangesAsync(), FindAsync(id); and other asyn methods
using Microsoft.AspNetCore.Mvc;    // allow to use ControllerBase
using Microsoft.AspNetCore.Authorization;   // To use AllowAnonymous Attribute
using Microsoft.AspNetCore.Diagnostics;   // to use IExceptionHandlerFeature

namespace api.Controllers
{

    [Route("ErrorHandling")]  //<-- starting URL for this Controller 
    [ApiController]
    [AllowAnonymous]   //Don't neet Authorization for this Class
    [ApiExplorerSettings(IgnoreApi = true)]  //Sqgger will not try to form Interface for all endpoint of this controller (this Attribute remove some errors)
    public class ErrorHandlingController : ControllerBase
    {


        [Route("ProcessError")]  //<--endpoint will be - [Route("api/ErrorHandling")] + ProcessError
        
        //New Method which will process all errors in separate Controller, where we return Problem object, its an object from ControllersBase, which will return all details about a problem, when error occur
        //to make all the errors porccessing in this controller --> we need to add some code in Program.cs (-->See Program.cs line 41)
        // public ActionResult ProcessError()=> Problem();   //<-- this will return structured and shorten result in error hadling in Swagger, Response body




        //Here We adjust this error hadling For Development and Production Environment
        //If application is in Production Environment we will return special result --> Problem();
        //If application is in Deevelopment Environment we will return full error details
         public ActionResult ProcessError([FromServices] IHostEnvironment hostEnvironment){

            if(hostEnvironment.IsDevelopment()){
            //custom logic , return full error details
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            return Problem(
                detail: feature.Error.StackTrace,
                title: feature.Error.Message,
                instance: hostEnvironment.EnvironmentName
            );

            }else
            {   
                //return limited error details
                return Problem();  //<--in Production Environment, will return structured and shorten result in error hadling in Swagger, Response body
            }


         }
    

    }
}