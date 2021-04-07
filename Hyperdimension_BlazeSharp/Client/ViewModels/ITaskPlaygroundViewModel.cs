using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using BlazorMonaco;
using Hyperdimension_BlazeSharp.Shared.Dto;
using Microsoft.AspNetCore.Components;

namespace Hyperdimension_BlazeSharp.Client.ViewModels
{
    public interface ITaskPlaygroundViewModel : INotifyPropertyChanged
    {
        public Guid TaskId { get; set; }
        public string Output { get; set; }
        public string CompileText { get; set; }
        public string Instruction { get; set; }
        public TaskDataPlayground TaskDataPlayground { get; set; }
        public Mode Mode { get; set; }
        public string EditorPosition { get; set; }
        public string Title { get; set; }
        public MonacoEditor Editor { get; set; }
        public int? Points { get; set; }
        public bool IsPreviousVersion { get; set; }
        

        public void Execute();
        public void ChangeEditorPosition();
        public StandaloneEditorConstructionOptions EditorConstructionOptions(MonacoEditor editor);
        public Task ChangeTheme(ChangeEventArgs e);
        public Task GetValue();
        public Task SetValue(string value);
        public Task RestorePreviousVersion();
        public Task GetTask();
        public void CheckIfDraftExists();
    }

    public enum Mode
    {
        Tutorial,
        Adventure
    }
}
