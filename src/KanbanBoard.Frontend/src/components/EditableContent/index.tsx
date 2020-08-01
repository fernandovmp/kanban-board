import React, { useState } from 'react';
import { Input } from '..';

interface IEditableContentProps {
    onEndEdit(value: string): void;
    initialInputValue?: string;
}

export const EditableContent: React.FC<IEditableContentProps> = ({
    initialInputValue,
    onEndEdit,
    children,
}) => {
    const [inputValue, setInputValue] = useState('');
    const [isEditing, setIsEditing] = useState(false);

    const handleBeginEditBoardTitle = () => {
        setInputValue(initialInputValue ?? '');
        setIsEditing(true);
    };

    const handleEditBoardTitle = () => {
        setIsEditing(false);
        onEndEdit(inputValue);
    };

    return (
        <div onClick={handleBeginEditBoardTitle}>
            {isEditing ? (
                <Input
                    value={inputValue}
                    onChange={(e) => setInputValue(e.target.value)}
                    onBlur={handleEditBoardTitle}
                    autoFocus
                />
            ) : (
                children
            )}
        </div>
    );
};
