import styled from 'styled-components';
import {
    DefaultButton,
    PrimaryButton,
    ResizableTextArea,
} from '../../../../components';

export const DescriptionCard = styled.div`
    margin-top: 12px;
    width: 100%;
    padding: 12px;
    background: white;
    border-radius: 8px;
    min-height: 100px;
    font-size: 14px;
`;

export const DescriptionTextArea = styled(ResizableTextArea)`
    margin-top: 12px;
    width: 100%;
    padding: 12px;
    background: white;
    border-radius: 8px;
    font-size: 14px;
    border: none;
    &:focus {
        outline: none;
    }
`;

export const ButtonsWrapper = styled.div`
    margin-top: 12px;
    display: flex;
    gap: 10px;
    justify-content: flex-end;
`;

export const CancelButton = styled(DefaultButton)`
    width: 120px;
    height: 25px;
    font-size: small;
    justify-content: center;
`;

export const SaveButton = styled(PrimaryButton)`
    width: 120px;
    height: 25px;
    font-size: small;
    justify-content: center;
`;
