﻿using Hyperdimension_BlazeSharp.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client
{
    public class TasksHistoryDraft : ObservableObject
    {
        public class DraftRecord
        {
            public string Code { get; set; }
            public string Title { get; set; }
        }

        private bool _isHidden = true;
        private readonly ILocalStorageService _localStorage;
        private readonly string storageItemName = "taskDraftContainer";

        public Dictionary<Guid, DraftRecord> Drafts { get; set; } = new();
        public bool IsHidden { get => _isHidden; set => OnPropertyChanged(ref _isHidden, value); }

        public TasksHistoryDraft(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            Task.Run(() => UpdateLocalDictionary());
        }

        public async Task AddDraft((Guid id, string title, string code) draft)
        {
            Drafts[draft.id] = new() { Title = draft.title, Code = draft.code };

            await _localStorage.SetItem(storageItemName, Drafts);

            OnPropertyChanged(nameof(Drafts));
        }

        public async Task RemoveDraft(Guid id)
        {
            if (CheckIfDraftExists(id))
            {
                Drafts.Remove(id);
                await _localStorage.SetItem(storageItemName, Drafts);
                OnPropertyChanged(nameof(Drafts));
            }
        }

        public string GetDraftCode(Guid id)
        {
            if (Drafts.TryGetValue(id, out DraftRecord record))
            {
                return record.Code;
            }
            else
            {
                return null;
            }
        }

        public bool CheckIfDraftExists(Guid id) => Drafts.TryGetValue(id, out _);        

        public int Count() => Drafts.Count;        

        public void ChangeVisibility() => IsHidden = !IsHidden;

        private async Task UpdateLocalDictionary()
            => Drafts = await _localStorage.GetItem<Dictionary<Guid, DraftRecord>>(storageItemName) ?? new();
    }    
}
