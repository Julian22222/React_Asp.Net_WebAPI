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

    Task<Comment?> GetCommentById(int id);  //Comment? <-- Can have a Comment or CAN BE NULL, if it is not find something it will return null

    Task<Comment> CreateComment(Comment commentModel);

    Task<Comment?> UpdateComment(int commentId, Comment commentModel);    //Comment? <-- Can have a Comment or CAN BE NULL


    Task<Comment?> DeleteComment(int commentId);

}
