import React, { useState } from 'react';
import { Route, useHistory } from 'react-router-dom';
import { FormField } from '../../components';
import { User } from '../../models';
import { apiPost, isErrorResponse } from '../../services/kanbanApiService';
import {
    mapApiErrorsToValidationErrors,
    ValidationError,
} from '../../validations';
import {
    AuthPageLink,
    AuthPageWrapper,
    ErrorMessageText,
    Main,
    SubmitButton,
    Title,
} from './styles';
import { FormData, loginSchema, signUpSchema } from './validations';

const defaultFormData: FormData = {
    name: '',
    email: '',
    password: '',
    confirmPassword: '',
};

type LoginResponse = {
    token: string;
    user: User;
};

export const AuthPage: React.FC = () => {
    const [errorMessage, setErrorMessage] = useState('');
    const [formData, setFormData] = useState(defaultFormData);
    const [errors, setErrors] = useState<ValidationError[]>([]);
    const history = useHistory();

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (window.location.pathname === '/login') {
            loginSchema
                .validate(formData, {
                    abortEarly: false,
                })
                .then(() => {
                    handleLogin();
                })
                .catch((err) => {
                    setErrors(
                        err.inner.map((error: any) => ({
                            path: error.path,
                            errors: error.errors,
                        }))
                    );
                });
            return;
        }
        signUpSchema
            .validate(formData, {
                abortEarly: false,
            })
            .then(() => {
                handleSignUp();
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

    const handleLogin = async () => {
        const response = await apiPost<LoginResponse>({
            uri: 'v1/login',
            body: {
                email: formData.email,
                password: formData.password,
            },
        });
        if (isErrorResponse(response.data)) {
            setErrorMessage(response.data.message);
            const apiErrors = response.data.errors ?? [];
            setErrors(mapApiErrorsToValidationErrors(apiErrors));
            return;
        }

        sessionStorage.setItem('jwtToken', response.data!.token);

        history.push('/');
    };
    const handleSignUp = async () => {
        const response = await apiPost({
            uri: 'v1/users',
            body: {
                name: formData.name ?? '',
                email: formData.email,
                password: formData.password,
                confirmPassword: formData.confirmPassword ?? '',
            },
        });
        if (isErrorResponse(response.data)) {
            setErrorMessage(response.data.message);
            const apiErrors = response.data.errors ?? [];
            setErrors(mapApiErrorsToValidationErrors(apiErrors));
            return;
        }
        handleLogin();
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
                {errorMessage && (
                    <ErrorMessageText>{errorMessage}</ErrorMessageText>
                )}
                <Route exact path="/signup">
                    <FormField
                        name="name"
                        onValueChange={handleInputChange}
                        placeholder="Name"
                        value={formData.name}
                        validationErrors={errors.filter(
                            (err) => err.path === 'name'
                        )}
                    />
                </Route>

                <FormField
                    name="email"
                    onValueChange={handleInputChange}
                    placeholder="E-mail"
                    value={formData.email}
                    validationErrors={errors.filter(
                        (err) => err.path === 'email'
                    )}
                />

                <FormField
                    name="password"
                    onValueChange={handleInputChange}
                    placeholder="Password"
                    type="password"
                    value={formData.password}
                    validationErrors={errors.filter(
                        (err) => err.path === 'password'
                    )}
                />

                <Route exact path="/signup">
                    <FormField
                        name="confirmPassword"
                        onValueChange={handleInputChange}
                        placeholder="Confirm Password"
                        type="password"
                        value={formData.confirmPassword}
                        validationErrors={errors.filter(
                            (err) => err.path === 'confirmPassword'
                        )}
                    />

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
