﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeworkBot.Models;


namespace HomeworkBot.Services;

public interface IStorage
{
    /// <summary>
    /// Получение сессии пользователя по идентификатору
    /// </summary>
    Session GetSession(long chatId);
}

