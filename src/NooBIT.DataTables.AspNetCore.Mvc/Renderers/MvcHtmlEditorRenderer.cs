﻿using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using NooBIT.DataTables.Editors;

namespace NooBIT.DataTables.AspNetCore.Mvc.Renderers
{
    internal class MvcHtmlEditorRenderer
    {
        public IHtmlContent Render(Editor editor)
        {
            switch (editor.EditorType)
            {
                case EditorType.Text:
                    return RenderTextBox(editor as TextBox);

                case EditorType.Date:
                    return RenderDatePicker(editor as DatePicker);

                case EditorType.Select:
                    return RenderSelect(editor as Select);

                case EditorType.LinkedSelect:
                    return RenderLinkedSelect(editor as LinkedSelect);

                case EditorType.CheckBox:
                    return RenderCheckBox(editor as CheckBox);

                case EditorType.Button:
                    return RenderButton(editor as Button);

                case EditorType.Number:
                    return RenderNumberTextBox(editor as NumberTextBox);
            }

            throw new NotImplementedException();
        }

        private IHtmlContent RenderLinkedSelect(LinkedSelect linkedSelect)
        {
            var select = new TagBuilder("select");

            SetDefaultAttributes(select, linkedSelect);
            foreach (var value in linkedSelect.FilterValues)
            {
                var option = new TagBuilder("option");
                option.InnerHtml.Append(value.Text);
                option.Attributes.Add("value", value.Value);
                option.Attributes.Add("data-ldo", value.LinkedValue);
                if (value.Selected)
                    option.Attributes.Add("selected", "selected");
                select.InnerHtml.AppendHtml(option);
            }

            return select;
        }

        private IHtmlContent RenderNumberTextBox(NumberTextBox numberTextBox)
        {
            var input = new TagBuilder("input");

            SetDefaultAttributes(input, numberTextBox);

            if (numberTextBox.MinValue.HasValue)
                input.Attributes.Add("min", numberTextBox.MinValue.Value.ToString());

            if (numberTextBox.MaxValue.HasValue)
                input.Attributes.Add("max", numberTextBox.MaxValue.Value.ToString());

            if (!string.IsNullOrWhiteSpace(numberTextBox.PlaceHolder))
                input.Attributes.Add("placeholder", numberTextBox.PlaceHolder);

            return input.RenderSelfClosingTag();
        }

        private IHtmlContent RenderButton(Button button)
        {
            var btn = new TagBuilder("button");

            SetDefaultAttributes(btn, button);

            switch (button.ButtonType)
            {
                case ButtonType.Button:
                    btn.Attributes.Add("type", "button");
                    break;
                case ButtonType.Reset:
                    btn.Attributes.Add("type", "reset");
                    break;
                case ButtonType.Submit:
                    btn.Attributes.Add("type", "submit");
                    break;
            }

            btn.InnerHtml.Append(button.Text);

            if (!string.IsNullOrWhiteSpace(button.OnClick))
                btn.Attributes.Add("onclick", button.OnClick);

            return btn;
        }

        private IHtmlContent RenderCheckBox(CheckBox checkBox)
        {
            var cb = new TagBuilder("input");
            SetDefaultAttributes(cb, checkBox);
            cb.Attributes.Add("type", "checkbox");
            return cb.RenderSelfClosingTag();
        }

        private IHtmlContent RenderSelect(Select sel)
        {
            var select = new TagBuilder("select");
            SetDefaultAttributes(select, sel);
            foreach (var value in sel.FilterValues)
            {
                var option = new TagBuilder("option");
                option.InnerHtml.Append(value.Text);
                option.Attributes.Add("value", value.Value);
                if (value.Selected)
                    option.Attributes.Add("selected", "selected");
                select.InnerHtml.AppendHtml(option);
            }

            return select;
        }


        private IHtmlContent RenderDatePicker(DatePicker datePicker)
        {
            var input = new TagBuilder("input");

            SetDefaultAttributes(input, datePicker);

            input.Attributes.Add("type", "text");

            if (!string.IsNullOrWhiteSpace(datePicker.PlaceHolder))
                input.Attributes.Add("placeholder", datePicker.PlaceHolder);

            if (!string.IsNullOrWhiteSpace(datePicker.DefaultValue))
                input.Attributes.Add("value", datePicker.DefaultValue);

            input.AddCssClass("datepicker");

            return input.RenderSelfClosingTag();
        }

        private IHtmlContent RenderTextBox(TextBox textBox)
        {
            var input = new TagBuilder("input");

            SetDefaultAttributes(input, textBox);

            input.Attributes.Add("type", "text");

            if (!string.IsNullOrWhiteSpace(textBox.PlaceHolder))
                input.Attributes.Add("placeholder", textBox.PlaceHolder);

            if (!string.IsNullOrWhiteSpace(textBox.DefaultValue))
                input.Attributes.Add("value", textBox.DefaultValue);

            return input.RenderSelfClosingTag();
        }

        private void SetDefaultAttributes(TagBuilder tagBuilder, Editor editor)
        {
            if (!string.IsNullOrWhiteSpace(editor.Id))
                tagBuilder.Attributes.Add("id", editor.Id);

            if (!string.IsNullOrWhiteSpace(editor.Name))
                tagBuilder.Attributes.Add("name", editor.Name);

            if (!string.IsNullOrWhiteSpace(editor.Title))
                tagBuilder.Attributes.Add("title", editor.Title);

            if (!string.IsNullOrWhiteSpace(editor.Classes))
                Array.ForEach(editor.Classes.Split(' '), tagBuilder.AddCssClass);

            if (editor.Disabled)
                tagBuilder.Attributes.Add("disabled", "disabled");

            if (editor.Required)
                tagBuilder.Attributes.Add("required", "required");
        }
    }
}