using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client
{
    public class TasksHistoryDraft
    {
        public Dictionary<Guid, string> Drafts { get; set; } = new();

        public void AddDraft(KeyValuePair<Guid, string> draft) => Drafts[draft.Key] = draft.Value;

        public void RemoveDraft(Guid id)
        {
            if (CheckIfDraftExists(id))
            {
                Drafts.Remove(id);
            }
        }

        public string GetDraft(Guid id)
        {
            if (Drafts.TryGetValue(id, out string code))
            {
                return code;
            }
            else
            {
                return null;
            }
        }

        public bool CheckIfDraftExists(Guid id) => Drafts.TryGetValue(id, out _);
    }
}
