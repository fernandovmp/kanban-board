import React, { useState } from 'react';
import { Route } from 'react-router-dom';
import {
    AuthInput,
    AuthPageLink,
    AuthPageWrapper,
    Main,
    SubmitButton,
    Title,
} from './styles';

type FormData = {
    name?: string;
    email: string;
    password: string;
    confirmPassword?: string;
};

const defaultFormData: FormData = {
    name: '',
    email: '',
    password: '',
    confirmPassword: '',
};

export const AuthPage: React.FC = () => {
    const [formData, setFormData] = useState(defaultFormData);

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
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
                </Route>

                <AuthInput
                    name="email"
                    placeholder="E-mail"
                    value={formData.email}
                    onChange={handleInputChange}
                />

                <AuthInput
                    name="password"
                    placeholder="Password"
                    value={formData.password}
                    onChange={handleInputChange}
                />

                <Route exact path="/signup">
                    <AuthInput
                        name="confirmPassword"
                        placeholder="Confirm Password"
                        value={formData.confirmPassword}
                        onChange={handleInputChange}
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
