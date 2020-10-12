import React from 'react';
import { ValidationError } from '../../validations';
import { FormInput, InputError, Label } from './styles';

interface IFormFieldProps {
    label?: string;
    name: string;
    onValueChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    placeholder?: string;
    validationErrors: ValidationError[];
    value?: string;
    type?: string;
}

export const FormField: React.FC<IFormFieldProps> = ({
    name,
    onValueChange,
    validationErrors,
    label,
    placeholder,
    type,
    value,
}) => {
    return (
        <Label>
            {label}
            <FormInput
                name={name}
                placeholder={placeholder}
                value={value}
                onChange={onValueChange}
                type={type}
            />
            {validationErrors.length > 0 &&
                validationErrors.map((err, index) => (
                    <InputError key={index}>{err.errors[0]}</InputError>
                ))}
        </Label>
    );
};
