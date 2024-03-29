﻿using BlazorMonaco;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public class TaskPlaygroundViewModel : ObservableObject, ITaskPlaygroundViewModel
    {
        private string _output;
        private string _compileText;        
        private int? _points;
        private bool _isPassed;
        private bool _isExecuting;
        private string _instruction;
        private Mode _mode = 0;        
        private string _title;
        private bool _isLastSendVersionPassingTests;
        private MonacoEditor _editor;


        public Guid TaskId { get; set; }
        public string Output 
        { 
            get => _output; 
            set => OnPropertyChanged(ref _output, value.Replace("\n", "<br />")); 
        }
        public string CompileText 
        { 
            get => _compileText; 
            set => OnPropertyChanged(ref _compileText, value); 
        }
        public string Instruction 
        { 
            get => _instruction; 
            set => OnPropertyChanged(ref _instruction, value); 
        }        
        public Mode Mode 
        { 
            get => _mode; 
            set => OnPropertyChanged(ref _mode, value); 
        }
        public string Title 
        { 
            get => _title; 
            set => OnPropertyChanged(ref _title, value); 
        }
        public MonacoEditor Editor 
        { 
            get => _editor; 
            set => OnPropertyChanged(ref _editor,value); 
        }
        public int? Points 
        { 
            get => _points; 
            set => OnPropertyChanged(ref _points, value); 
        }
        public bool IsPassed
        {
            get => _isPassed;
            set => OnPropertyChanged(ref _isPassed, value);
        }
        public bool IsExecuting 
        { 
            get => _isExecuting; 
            set => OnPropertyChanged(ref _isExecuting, value); 
        }
        public bool IsLastSendVersionPassingTests
        {
            get => _isLastSendVersionPassingTests;
            set => OnPropertyChanged(ref _isLastSendVersionPassingTests, value);
        }
        public string CopyOfLastExecutedVersion { get; set; }
        public TaskDataPlayground TaskDataPlayground { get; set; }

        private readonly TasksHistoryDraft _tasksHistoryDraft;
        private readonly CompileService _compileService;
        private readonly HttpClient _httpClient;
        private string _testCode;
        private string _initialCode;
        

        public TaskPlaygroundViewModel() { }
        public TaskPlaygroundViewModel(HttpClient httpClient, TasksHistoryDraft tasksHistoryDraft, CompileService compileService)
        {
            _httpClient = httpClient;
            _tasksHistoryDraft = tasksHistoryDraft;
            _compileService = compileService;
        }

        public async Task ChangeTheme(ChangeEventArgs e)
        {
            await MonacoEditor.SetTheme(e.Value.ToString());
        }

        public StandaloneEditorConstructionOptions EditorConstructionOptions(MonacoEditor editor)
        {
            return new StandaloneEditorConstructionOptions
            {
                AutomaticLayout = true,
                Language = "csharp",
                Value = string.Empty
            };
        }

        public async Task Execute()
        {
            var code = await GetValue();
            CopyOfLastExecutedVersion = code;
            await _tasksHistoryDraft.AddDraft(new(TaskId, Title, code));
            IsExecuting = true;

            try
            {
                _compileService.CompileLog = new List<string>();

                code += _testCode;

                var tmp = await _compileService.CompileAndRun(code);
                Output = tmp.Item2;
                IsPassed = tmp.Item1;

                if (IsPassed)
                {
                    Output += Environment.NewLine + @"<span class=""text-success"">[Task Passed!]</span>";
                }
                else
                {
                    Output += Environment.NewLine + @"<span class=""text-danger"">[Failure]</span>";
                }
            }
            catch (Exception e)
            {
                _compileService.CompileLog.Add(e.Message);
                _compileService.CompileLog.Add(e.StackTrace);
            }
            finally
            {
                CompileText = string.Join("<br />", _compileService.CompileLog);
                IsExecuting = false;
            }
        }

        public async Task<string> GetValue()
        {
            return await Editor.GetValue();
        }

        public async Task RestorePreviousVersion()
        {
            await SetValue(_tasksHistoryDraft.GetDraftCode(TaskId));            
        }

        public async Task SetValue(string value)
        {
            await Editor.SetValue(value);
        }

        public async Task GetTask()
        {
            var task = await _httpClient.GetFromJsonAsync<TaskDataPlayground>($"tasks/{TaskId}");
            if(task is not null)
            LoadCurrentObject(task);
            await SetValue(_initialCode);
        }

        public async Task CheckIfLastSendVerisonIsPassingTests(Guid userId, Guid taskId)
        {
            if (userId != Guid.Empty && taskId != Guid.Empty)
            {
                IsLastSendVersionPassingTests = await _httpClient.GetFromJsonAsync<bool>($"/tasks/ispassed?userId={userId}&taskId={taskId}");
            }
        }

        private void LoadCurrentObject(TaskPlaygroundViewModel taskPlaygroundViewModel)
        {
            Instruction = taskPlaygroundViewModel.Instruction;
            Title = taskPlaygroundViewModel.Title;
            Output = taskPlaygroundViewModel.Output;
            Mode = taskPlaygroundViewModel.Mode;
            Points = taskPlaygroundViewModel.Points;
            _testCode = taskPlaygroundViewModel._testCode;
            _initialCode = taskPlaygroundViewModel._initialCode;
        }

        public static implicit operator TaskPlaygroundViewModel(TaskDataPlayground taskDataPlayground)
        {
            return new()
            {
                TaskId = taskDataPlayground.Guid,
                Instruction = taskDataPlayground.Description,
                Title = taskDataPlayground.Title,
                Output = string.Empty,
                Mode = (Mode)taskDataPlayground.Mode,
                Points = taskDataPlayground.Points,
                _testCode = taskDataPlayground.TestCode,
                _initialCode = taskDataPlayground.InitialCode
            };
        }
    }
}
