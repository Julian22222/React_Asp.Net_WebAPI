using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;   //Can you Task with async await , and to use Task
using Microsoft.EntityFrameworkCore;  //to use ToListAsync method, SaveChangesAsync(), FindAsync(id); and other asyn methods
using api.Models;
using api.Interfaces;  //To use ICommentRepository
using api.Data;


namespace api.Repository;

public class CommentRepository : ICommentRepository
{

    private readonly ApplicationDBContext _context;   //assign a variable

    //ctor + tab ,   <--constructor
    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }


    public async Task<List<Comment>> GetAllComments(){
        return await _context.Comments.ToListAsync();
    }



}
