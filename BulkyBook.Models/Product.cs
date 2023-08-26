using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BulkyBook.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    [Required]
    public string ISBN { get; set; }

    [Required]
    public string Author { get; set; }

    [Required]
    [Range(1, 10000)]
    [Display(Name = "List Price")]
    public double ListPrice { get; set; }

    [Required]
    [Range(1, 10000)]
    [Display(Name = "Price for 1-50")]
    public double Price { get; set; }

    [Required]
    [Range(1, 10000)]
    [Display(Name = "Price for 51-100")]
    public double Price50 { get; set; }

    [Required]
    [Range(1, 10000)]
    [Display(Name = "Price for 100+")]
    public double Price100 { get; set; }

    [ValidateNever]
    public string ImageUrl { get; set; }

    [Required]
    [Display(Name = "Category Id")]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    [ValidateNever]
    public Category Category { get; set; }

    [Required]
    [Display(Name = "Cover Type Id")]
    public int CoverTypeId { get; set; }

    [ForeignKey(nameof(CoverTypeId))]
    [ValidateNever]
    [Display(Name = "Cover Type")]
    public CoverType CoverType { get; set; }

}
