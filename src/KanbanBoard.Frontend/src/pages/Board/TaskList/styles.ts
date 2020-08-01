import styled from 'styled-components';
import { Card, DefaultButton, PrimaryButton } from '../../../components';

export const ListWrapper = styled(Card)`
    background: #f0f0f0;
    width: 240px;
    padding: 10px 6px;
    display: flex;
    flex-direction: column;
    gap: 10px;
`;

export const ListTitle = styled.h3`
    margin: 0;
`;

export const Button = styled(DefaultButton)`
    background: transparent;
    font-size: small;
`;

export const NewTaskInput = styled.textarea`
    border: none;
    resize: vertical;
    max-height: 200px;
    &:focus {
        outline: none;
    }
`;

export const ButtonsWrapper = styled.div`
    display: flex;
    gap: 10px;
    justify-content: flex-end;
`;

export const CancelButton = styled(DefaultButton)`
    background: transparent;
    width: 80px;
    font-size: small;
    justify-content: center;

    &:hover {
        background: #e4e4e4;
    }
`;

export const SaveButton = styled(PrimaryButton)`
    width: 80px;
    font-size: small;
    justify-content: center;
`;
