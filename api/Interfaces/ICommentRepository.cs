using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;   //Can you Task with async await , and to use Task
using Microsoft.EntityFrameworkCore;  //to use ToListAsync method, SaveChangesAsync(), FindAsync(id); and other asyn methods
using api.Models;

namespace api.Interfaces;

public interface ICommentRepository
{

    //This Interface has the same Methods and properties as CommentRepository Class in Repository folder

    Task<List<Comment>> GetAllComments();

    Task<Comment?> GetCommentById(int id);  //Coment is optional, Get the coment by Id 

    Task<Comment> CreateComment(Comment commentModel);
}
