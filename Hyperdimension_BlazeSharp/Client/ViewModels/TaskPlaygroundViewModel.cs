using BlazorMonaco;
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
        private Guid _taskId;
        private int? _points;
        private bool _isPreviousVersion = false;
        private string _instruction;
        private Mode _mode = 0;
        private string _editorPosition = "col-md-6";
        private string _title;
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
            set => _mode = value; 
        }
        public string EditorPosition 
        { 
            get => _editorPosition;
            set => OnPropertyChanged(ref _editorPosition, value); 
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
        public bool IsPreviousVersion 
        { 
            get => _isPreviousVersion; 
            set => OnPropertyChanged(ref _isPreviousVersion, value); 
        }
        public TaskDataPlayground TaskDataPlayground { get; set; }

        private TasksHistoryDraft _tasksHistoryDraft;
        private CompileService _compileService;
        private HttpClient _httpClient;
        private string _testCode;
        private string _initialCode;
        

        public TaskPlaygroundViewModel() { }
        public TaskPlaygroundViewModel(HttpClient httpClient, TasksHistoryDraft tasksHistoryDraft, CompileService compileService)
        {
            _httpClient = httpClient;
            _tasksHistoryDraft = tasksHistoryDraft;
            _compileService = compileService;
        }

        public void ChangeEditorPosition()
        {
            EditorPosition = EditorPosition == "col-md-6" ? "col-md-12" : "col-md-6";
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
            var code = await Editor.GetValue();
            _tasksHistoryDraft.AddDraft(new(TaskId, Title, code));
            IsPreviousVersion = false;

            try
            {
                _compileService.CompileLog = new List<string>();

                #region tmp solution
                var index = code.IndexOf("class Program");

                code = code.Insert(code.IndexOf('{', index) + 1, _testCode);
                code = code.Insert(0, "using System.Text;using System.IO;");
                #endregion

                Output = await _compileService.CompileAndRun(code);                
            }
            catch (Exception e)
            {
                _compileService.CompileLog.Add(e.Message);
                _compileService.CompileLog.Add(e.StackTrace);
                //throw;
            }
            finally
            {
                CompileText = string.Join("<br />", _compileService.CompileLog);                
            }
        }

        public Task GetValue()
        {
            throw new NotImplementedException();
        }

        public async Task RestorePreviousVersion()
        {
            await SetValue(_tasksHistoryDraft.GetDraftCode(TaskId));
            IsPreviousVersion = false;
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

        public void CheckIfDraftExists()
        {
            IsPreviousVersion = _tasksHistoryDraft.CheckIfDraftExists(TaskId);
        }

        private void LoadCurrentObject(TaskPlaygroundViewModel taskPlaygroundViewModel)
        {
            Instruction = taskPlaygroundViewModel.Instruction;
            Title = taskPlaygroundViewModel.Title;
            //Mode = taskDataPlayground.Mode
            Points = taskPlaygroundViewModel.Points;
            _testCode = taskPlaygroundViewModel._testCode;
            _initialCode = taskPlaygroundViewModel._initialCode;
        }

        public static implicit operator TaskPlaygroundViewModel(TaskDataPlayground taskDataPlayground)
        {
            return new()
            {
                _taskId = taskDataPlayground.Guid,
                Instruction = Markdig.Markdown.ToHtml(taskDataPlayground.Description),
                Title = taskDataPlayground.Title,
                //Mode = taskDataPlayground.Mode
                Points = taskDataPlayground.Points,
                _testCode = taskDataPlayground.TestCode,
                _initialCode = taskDataPlayground.InitialCode
            };
        }
    }
}
