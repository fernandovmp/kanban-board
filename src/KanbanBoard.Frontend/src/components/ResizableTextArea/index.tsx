import React, { useState } from 'react';
import styled from 'styled-components';

// Based on https://codepen.io/liborgabrhel/pen/eyzwOx

const TextArea = styled.textarea`
    resize: none;
    line-height: 18px;
    overflow: auto;
    height: auto;
`;

interface IResizableTextAreaProps {
    className?: string;
    minRows?: number;
    maxRows?: number;
    value?: string;
    onChange?: (event: React.ChangeEvent<HTMLTextAreaElement>) => void;
    placeholder?: string;
    autoFocus?: boolean;
}

export function ResizableTextArea({
    autoFocus,
    className,
    maxRows,
    minRows,
    onChange,
    placeholder,
    value,
}: IResizableTextAreaProps) {
    const [rows, setRows] = useState(0);

    const resizeTextArea = (textarea: EventTarget & HTMLTextAreaElement) => {
        const trueMinRows = minRows ?? 1;
        const trueMaxRows = maxRows ?? 100;

        const lineHeight = 18;
        console.log(lineHeight);
        const previousRows = textarea.rows;
        textarea.rows = trueMinRows;
        const currentRows = Math.floor(textarea.scrollHeight / lineHeight);

        if (currentRows === previousRows) {
            textarea.rows = currentRows;
        }
        if (currentRows >= trueMaxRows) {
            textarea.rows = trueMaxRows;
            textarea.scrollTop = textarea.scrollHeight;
        }
        setRows(currentRows < trueMaxRows ? currentRows : trueMaxRows);
    };

    const handleFocus = (event: React.FocusEvent<HTMLTextAreaElement>) =>
        resizeTextArea(event.target);

    const handleChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
        resizeTextArea(event.target);
        if (onChange) {
            onChange(event);
        }
    };

    return (
        <TextArea
            className={className}
            rows={rows}
            onFocus={handleFocus}
            onChange={handleChange}
            value={value}
            placeholder={placeholder}
            autoFocus={autoFocus}
        />
    );
}
