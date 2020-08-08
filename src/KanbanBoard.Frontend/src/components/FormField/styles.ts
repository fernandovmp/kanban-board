import styled from 'styled-components';
import { Input } from '../Input';

export const FormInput = styled(Input)`
    background: #fafafa;
`;

export const InputError = styled.span`
    color: red;
    font-size: smaller;
`;

export const Label = styled.label`
    display: flex;
    flex-direction: column;
`;
