using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;   //Can you Task with async await , and to use Task
using Microsoft.EntityFrameworkCore;  //to use ToListAsync method, SaveChangesAsync(), FindAsync(id); and other asyn methods
using api.Models;
using Microsoft.AspNetCore.Mvc;  // allow to use ControllerBase

namespace api.Helpers;

public class QueryObject
{
    public string? Type { get; set; } = null;  // string? <--Type is optional

    public string? Name { get; set; } = null;   //string? Name is optional

    public string? SortBy { get; set; } = null;

    public bool IsDescending { get; set; } = false;



    //for Pagination
    public int PageNumber { get; set; } = 1;

    //how many Items show on one Page
    public int PageSize { get; set; } = 20;
}
