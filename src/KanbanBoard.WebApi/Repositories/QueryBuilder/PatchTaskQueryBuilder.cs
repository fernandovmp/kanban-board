using System;
using System.Collections.Generic;
using System.Linq;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Morcatko.AspNetCore.JsonMergePatch;

namespace KanbanBoard.WebApi.Repositories.QueryBuilder
{
    public class PatchTaskQueryBuilder : IPatchQueryBuilder<PatchTaskParams>
    {
        private readonly JsonMergePatchDocument<PatchTaskViewModel> _patchDocument;
        private readonly int _taskId;

        public PatchTaskQueryBuilder(JsonMergePatchDocument<PatchTaskViewModel> patchDocument, int taskId)
        {
            _patchDocument = patchDocument;
            _taskId = taskId;
        }

        public (string query, PatchTaskParams queryParams) Build()
        {
            var setFields = new List<string>();
            bool withListPatch = false;
            foreach (Operation<PatchTaskViewModel> operation in _patchDocument.Operations)
            {
                (string field, string paramName) = GetDatabaseFieldName(operation.path);
                if (field == "list_id")
                {
                    withListPatch = true;
                    continue;
                }

                setFields.Add(BuildSetInstruction(field, paramName));
            }

            string query = "";
            if (setFields.Count > 0)
            {
                query = $"update tasks set {string.Join(", ", setFields)} where id = @Id;";
            }
            if (withListPatch)
            {
                query += "update list_tasks set list_id = @List where task_id = @Id;";
            }

            PatchTaskViewModel patchModel = _patchDocument.Model;
            var queryParams = new PatchTaskParams
            {
                Id = _taskId,
                Summary = patchModel.Summary,
                Description = patchModel.Description,
                TagColor = patchModel.TagColor,
                List = patchModel.List
            };
            return (query, queryParams);
        }

        private (string fieldName, string paramName) GetDatabaseFieldName(string propertyPath) =>
            propertyPath switch
            {
                "/summary" => ("summary", $"@{nameof(PatchTaskParams.Summary)}"),
                "/description" => ("description", $"@{nameof(PatchTaskParams.Description)}"),
                "/tagColor" => ("tag_color", $"@{nameof(PatchTaskParams.TagColor)}"),
                "/list" => ("list_id", "@List"),
                _ => throw new Exception($"Property path \"{propertyPath}\" is not a valid property to build a patch query to {nameof(PatchTaskViewModel)}")
            };

        private string BuildSetInstruction(string field, string paramName) => $"{field} = {paramName}";
    }
}
