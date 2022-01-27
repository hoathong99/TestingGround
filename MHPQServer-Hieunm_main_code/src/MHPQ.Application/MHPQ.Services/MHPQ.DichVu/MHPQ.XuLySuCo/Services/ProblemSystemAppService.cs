using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using MHPQ.Authorization.Users;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using MHPQ.Services.Dto;
using System;
using System.Threading.Tasks;
using System.Linq;
using MHPQ.Authorization.Roles;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MHPQ.Services
{
    public interface IProblemSystemAppService : IApplicationService
    {
        Task<object> CreateOrUpdateProblem(ProblemSystemDto input);
        object UpdateProblems(List<ProblemSystemDto> input);
        Task<object> GetAllProblem();
        Task<object> GetProblemById(long id);
        //Task<object> GetByIdDevice(long id);
        Task<object> DeleteProblem(long id);
    }

    public class ProblemSystemAppService: MHPQAppServiceBase, IProblemSystemAppService
    {
        private readonly IRepository<ProblemSystem, long> _problemRepos;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Role, int> _roleRepository;
        private readonly RoleManager _roleManager;

        public ProblemSystemAppService(
            IRepository<ProblemSystem, long> problemRepos,
            IRepository<User, long> userRepository,
            IRepository<Role, int> roleRepository,
            RoleManager roleManager
            ) {
            _problemRepos = problemRepos;
            _userRepository = userRepository;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
        }

        [System.Obsolete]
        public async Task<object> CreateOrUpdateProblem(ProblemSystemDto input)
        {
            try
            {
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _problemRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);
                        await _problemRepos.UpdateAsync(updateData);

                    }
                    return 1;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<ProblemSystem>();
                    long id = await _problemRepos.InsertAndGetIdAsync(insertInput);
                    if (id > 0)
                    {

                        insertInput.Id = id;
                    }
                    return insertInput;
                }


            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return -1;
            }
        }
        [System.Obsolete]
        public object UpdateProblems(List<ProblemSystemDto> input)
        {
            try
            {

                if (input != null)
                {
                    input.ForEach(problem =>
                    {
                        var updateData =  _problemRepos.Get(problem.Id);
                        if (updateData != null)
                        {
                            problem.MapTo(updateData);
                             _problemRepos.Update(updateData);

                        }
                    });
                }

                return 1;

            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return -1;
            }
        }

        public Task<object> DeleteProblem(long id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<object> GetAllProblem()
        {
            try
            {
                var user = await _userRepository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == AbpSession.UserId);
                var role =  _roleManager.Roles.Where(x => x.Name.Trim() == "Xử lý sự cố").FirstOrDefault();
                

                var query = (from pb in _problemRepos.GetAll()
                             join pf in _userRepository.GetAll() on pb.PerformerId equals pf.Id into plist from pf in plist.DefaultIfEmpty()
                             join gv in _userRepository.GetAll() on pb.GiverId equals gv.Id into glist from gv in glist.DefaultIfEmpty()
                             join cs in _userRepository.GetAll() on pb.CreatorUserId equals cs.Id into clist from cs in clist.DefaultIfEmpty()
                             select new ProblemSystemDto
                             {
                                 Id = pb.Id,
                                 GiverId = pb.GiverId,
                                 PerformerId = pb.PerformerId,
                                 CreationTime = pb.CreationTime,
                                 Description = pb.Description,
                                 LastModificationTime = pb.LastModificationTime,
                                 Name = pb.Name,
                                 NameGiver = gv.FullName,
                                 NamePerformer = pf.FullName,
                                 State = pb.State,
                                 TimeFinish = pb.TimeFinish,
                                 TimeStart = pb.TimeStart,
                                 TypeProblem = pb.TypeProblem,
                                 NameCustomer = cs.FullName,
                                 Address = pb.Address,
                                 CreatorUserId = pb.CreatorUserId   
                             });

                if (role != null)
                {
                    var check = user.Roles.Where(x => x.RoleId == role.Id).FirstOrDefault();
                }
               
                if (role == null)
                {
                    var result = query.ToList();
                    return result;
                }
                else
                {
                    var result = await query.Where(x => x.PerformerId == AbpSession.UserId).ToListAsync();
                    return result;
                }
               
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }
        }

        public async Task<object> GetProblemById(long id)
        {
            try
            {
                
                var query = (from pb in _problemRepos.GetAll()
                                 //join pf in _userRepository.GetAll() on pb.PerformerId equals pf.Id into plist from pf in plist.DefaultIfEmpty()
                                 //join gv in _userRepository.GetAll() on pb.GiverId equals gv.Id into glist from gv in glist.DefaultIfEmpty()
                             join cs in _userRepository.GetAll() on pb.CreatorUserId equals cs.Id into clist
                             from cs in clist.DefaultIfEmpty()
                             select new ProblemSystemDto
                             {
                                 Id = pb.Id,
                                 GiverId = pb.GiverId,
                                 PerformerId = pb.PerformerId,
                                 CreationTime = pb.CreationTime,
                                 Description = pb.Description,
                                 LastModificationTime = pb.LastModificationTime,
                                 Name = pb.Name,
                                 // NameGiver = gv.FullName,
                                 // NamePerformer = pf.FullName,
                                 State = pb.State,
                                 TimeFinish = pb.TimeFinish,
                                 TimeStart = pb.TimeStart,
                                 TypeProblem = pb.TypeProblem,
                                 NameCustomer = cs.FullName,
                                 Address = pb.Address,
                                 CreatorUserId = pb.CreatorUserId
                             });
                var result = await query.FirstOrDefaultAsync(x => x.Id == id);

                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;

            }
        }
    }
}
