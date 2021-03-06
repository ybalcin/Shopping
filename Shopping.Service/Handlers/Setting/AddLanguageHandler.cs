﻿using Shopping.Data;
using Shopping.Model.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Service.Handlers.Setting
{
    public class AddLanguageHandler : CommandHandler<Commands.AddLanguage>
    {
        private readonly IRepository<Language> languageRepository;
        private readonly IUnitOfWork unitOfWork;
        public AddLanguageHandler(IUnitOfWork unitOfWork, IRepository<Language> languageRepository)
        {
            this.unitOfWork = unitOfWork;
            this.languageRepository = languageRepository;
        }
        public override async Task<dynamic> HandleAsync(Commands.AddLanguage command)
        {
            Result result;
            // validate the command
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                result = new Result(false, command.Name, "Ad gereklidir.", true, null);
                return await Task.FromResult(result);
                
            }
            if (command.Name.Length > 200)
            {
                result = new Result(false, command.Name, "Ad 200 karakterden uzun olamaz.", true, null);
                return await Task.FromResult(result);
            }
            if (string.IsNullOrWhiteSpace(command.NativeName))
            {
                result = new Result(false, command.NativeName, "Yerel Ad gereklidir.", true, null);
                return await Task.FromResult(result);
            }
            if (command.NativeName.Length > 200)
            {
                result = new Result(false, command.NativeName, "Yerel Ad 200 karakterden uzun olamaz.", true, null);
                return await Task.FromResult(result);
            }
            if (string.IsNullOrWhiteSpace(command.Flag))
            {
                result = new Result(false, command.Flag, "Bayrak gereklidir.", true, null);
                return await Task.FromResult(result);
            }
            if (command.Flag.Length > 200)
            {
                result = new Result(false, command.Flag, "Bayrak 200 karakterden uzun olamaz.", true, null);
                return await Task.FromResult(result);
            }

            // map command to the model
            var model = Mapper.Map<Language>(command);

            // mark the model to insert
            languageRepository.Insert(model);

            // save changes to database
            await unitOfWork.SaveChangesAsync();

            // return the result
            result =new Result( true,model.Id, "Dil başarıyla eklendi.", false, 1);

            return await Task.FromResult(result);
        }
    }
}
