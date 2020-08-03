import { Link } from 'react-router-dom';
import styled from 'styled-components';
import { Card, Input, PrimaryButton } from '../../components';

export const Main = styled.main`
    display: flex;
    justify-content: center;
`;

export const AuthPageWrapper = styled(Card)`
    width: 400px;
    margin-top: 20vh;
    display: flex;
    flex-direction: column;
    gap: 15px;
    padding: 15px;
`;

export const SubmitButton = styled(PrimaryButton)`
    height: 30px;
    justify-content: center;
`;

export const AuthPageLink = styled(Link)`
    align-self: center;
    color: #0069e4;
    font-size: small;
`;

export const AuthInput = styled(Input)`
    background: #fafafa;
`;

export const Title = styled.h1`
    align-self: center;
    margin-top: 0;
    margin-bottom: 30px;
`;

export const InputError = styled.span`
    color: red;
    font-size: smaller;
    margin-top: -15px;
`;
