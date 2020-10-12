import styled from 'styled-components';
import { DefaultButton, PrimaryButton } from '../Button';

export const ButtonsWrapper = styled.div`
    margin-top: 30px;
    display: flex;
    gap: 30px;
    justify-content: flex-end;
`;

export const CancelButton = styled(DefaultButton)`
    width: 120px;
    justify-content: center;
`;

export const ConfirmButton = styled(PrimaryButton)`
    width: 120px;
    justify-content: center;
`;

export const Header = styled.h3`
    margin: 0;
`;
