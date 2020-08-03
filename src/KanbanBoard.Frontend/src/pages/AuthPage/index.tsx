import React, { useState } from 'react';
import { Route } from 'react-router-dom';
import * as yup from 'yup';
import {
    AuthInput,
    AuthPageLink,
    AuthPageWrapper,
    InputError,
    Main,
    SubmitButton,
    Title,
} from './styles';

type LoginFormData = {
    email: string;
    password: string;
};

type FormData = {
    name?: string;
    confirmPassword?: string;
} & LoginFormData;

const defaultFormData: FormData = {
    name: '',
    email: '',
    password: '',
    confirmPassword: '',
};

const loginSchema = yup.object().shape<LoginFormData>({
    email: yup
        .string()
        .required('e-mail is required')
        .email('insert a valid email'),
    password: yup.string().trim().required('password is required').min(8),
});

const signUpSchema = yup.object().shape<FormData>({
    name: yup
        .string()
        .trim()
        .required('name is required')
        .max(100, "name length should be at maximum 100 character's"),
    email: yup
        .string()
        .required('e-mail is required')
        .email('insert a valid email'),
    password: yup.string().trim().required().min(8),
    confirmPassword: yup
        .string()
        .trim()
        .required()
        .min(8)
        .equals([yup.ref('password')], "passwords doesn't match"),
});

type ValidationError = {
    path: string;
    errors: string[];
};

export const AuthPage: React.FC = () => {
    const [formData, setFormData] = useState(defaultFormData);
    const [errors, setErrors] = useState<ValidationError[]>([]);

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const schemaToValidate =
            window.location.pathname === '/login' ? loginSchema : signUpSchema;
        schemaToValidate
            .validate(formData, {
                abortEarly: false,
            })
            .catch((err) => {
                setErrors(
                    err.inner.map((error: any) => ({
                        path: error.path,
                        errors: error.errors,
                    }))
                );
            });
    };

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const key = event.target.getAttribute('name');
        const { value } = event.target;

        if (!key) return;

        setFormData({
            ...formData,
            [key]: value,
        });
    };

    return (
        <Main>
            <AuthPageWrapper as="form" onSubmit={handleSubmit}>
                <Title>Kanban Board</Title>
                <Route exact path="/signup">
                    <AuthInput
                        name="name"
                        placeholder="Name"
                        value={formData.name}
                        onChange={handleInputChange}
                    />
                    {errors.filter((err) => err.path === 'name') &&
                        errors
                            .filter((err) => err.path === 'name')
                            .map((err, index) => (
                                <InputError key={index}>
                                    {err.errors[0]}
                                </InputError>
                            ))}
                </Route>

                <AuthInput
                    name="email"
                    placeholder="E-mail"
                    value={formData.email}
                    onChange={handleInputChange}
                />
                {errors.filter((err) => err.path === 'email') &&
                    errors
                        .filter((err) => err.path === 'email')
                        .map((err, index) => (
                            <InputError key={index}>{err.errors[0]}</InputError>
                        ))}

                <AuthInput
                    name="password"
                    placeholder="Password"
                    type="password"
                    value={formData.password}
                    onChange={handleInputChange}
                />
                {errors.filter((err) => err.path === 'password') &&
                    errors
                        .filter((err) => err.path === 'password')
                        .map((err, index) => (
                            <InputError key={index}>{err.errors[0]}</InputError>
                        ))}

                <Route exact path="/signup">
                    <AuthInput
                        name="confirmPassword"
                        placeholder="Confirm Password"
                        type="password"
                        value={formData.confirmPassword}
                        onChange={handleInputChange}
                    />
                    {errors.filter((err) => err.path === 'confirmPassword') &&
                        errors
                            .filter((err) => err.path === 'confirmPassword')
                            .map((err, index) => (
                                <InputError key={index}>
                                    {err.errors[0]}
                                </InputError>
                            ))}

                    <SubmitButton type="submit">SIGN UP</SubmitButton>
                    <AuthPageLink to="/login">
                        I already have an account
                    </AuthPageLink>
                </Route>
                <Route exact path="/login">
                    <SubmitButton type="submit">LOG IN</SubmitButton>
                    <AuthPageLink to="/signup">Create an account</AuthPageLink>
                </Route>
            </AuthPageWrapper>
        </Main>
    );
};
