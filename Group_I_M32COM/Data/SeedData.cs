﻿using Group_I_M32COM.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group_I_M32COM.Data
{
    public class SeedData
    {
        // The seed data expects three arguments: context, usermanager object and role manager object
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            context.Database.EnsureCreated(); // this is to ensure that the database is created 

            string adminId1 = string.Empty;
            string adminId2 = string.Empty;

            string role1 = "Admin";
            string desc1 = "This is the administrator role";

            string role2 = "Member";
            string desc2 = "This is the members role";

            string password = "P@$$w0rd";

            // To check if the admin role exists in the database
            /*if (await roleManager.FindByNameAsync(role1) == null)
            {
                // Instantiate the new admin role through the constructor linked in the Application Role class
                await roleManager.CreateAsync(new ApplicationRole(role1, desc1, DateTime.Now));
            }

            if (await roleManager.FindByNameAsync(role2) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role2, desc2, DateTime.Now));
            }*/

            List<Data_Roles> data_Roles = new List<Data_Roles>()
            {
                new Data_Roles { Role_Name = "Admin", Role_Description = "This is the administrator role"},
                new Data_Roles { Role_Name = "Member", Role_Description = "This is the members role"},
                new Data_Roles { Role_Name = "User", Role_Description = "This is the user role"}
            };

            foreach (var roles in data_Roles)
            {
                await CreateRole(roles.Role_Name, roles.Role_Description);
            }

            //await CreateRole("Admin", "This is the administrator role");
            //await CreateRole("Member", "This is the members role");
            //await CreateRole("User", "This is the user role");

            // A created method assigned to use role_name and role_description as a parameter to check if the role exists before inserting to database.
            async Task CreateRole(string role_name, string role_description)
            {
                if (!(await roleManager.RoleExistsAsync(role_name)))
                {
                    await roleManager.CreateAsync(new ApplicationRole(role_name, role_description, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim())));
                }
            }

            // To check if the user exists in the database
            if (await userManager.FindByNameAsync("rtesting@test.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "rtesting@test.com",
                    Email = "rtesting@test.com",
                    FirstName = "Adam",
                    LastName = "Aldridge",
                    Address = "Fake St",
                    City = "Vancouver",
                    PostalCode = "VSU K8I",
                    Country = "Canada",
                    PhoneNumber = "6902341234",
                    Created_At = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim())
                };

                // create the user through the user manager
                var result = await userManager.CreateAsync(user);
                // If the user is successfully created. We assign the user a password and a role through the user manager object.
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role1);
                }
                adminId1 = user.Id;
            }

            // To check if the second user exists in the database
            if (await userManager.FindByNameAsync("btesting@test.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "btesting@test.com",
                    Email = "btesting@test.com",
                    FirstName = "Bob",
                    LastName = "Parker",
                    Address = "Vermount St",
                    City = "Surrey",
                    PostalCode = "VSU K8I",
                    Country = "Canada",
                    PhoneNumber = "6702341234",
                    Created_At = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim())
                };

                // create the user through the user manager
                var result = await userManager.CreateAsync(user);
                // If the user is successfully created. We assign the user a password and a role through the user manager object.
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role1);
                }
                adminId1 = user.Id;
            }

            // to check for the third user
            if (await userManager.FindByNameAsync("ctesting@test.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "ctesting@test.com",
                    Email = "ctesting@test.com",
                    FirstName = "Smith",
                    LastName = "Aldridge",
                    Address = "Yew St",
                    City = "Vancouver",
                    PostalCode = "VSU K8I",
                    Country = "Canada",
                    PhoneNumber = "6905341234",
                    Created_At = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim())
                };

                // create the user through the user manager
                var result = await userManager.CreateAsync(user);
                // If the user is successfully created. We assign the user a password and a role through the user manager object.
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }

            // to check for the fourth user
            if (await userManager.FindByNameAsync("dtesting@test.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "dtesting@test.com",
                    Email = "dtesting@test.com",
                    FirstName = "Chris",
                    LastName = "Aldridge",
                    Address = "Fake St",
                    City = "Vancouver",
                    PostalCode = "VSU K8I",
                    Country = "Canada",
                    PhoneNumber = "6901521234",
                    Created_At = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim())
                };

                // create the user through the user manager
                var result = await userManager.CreateAsync(user);
                // If the user is successfully created. We assign the user a password and a role through the user manager object.
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }
        }

        public class Data_Roles
        {
            public string Role_Name { get; set; }
            public string Role_Description { get; set; }
        }
    }
}
