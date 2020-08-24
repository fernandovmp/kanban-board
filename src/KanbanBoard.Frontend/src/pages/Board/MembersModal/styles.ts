import styled from 'styled-components';
import { Input } from '../../../components';

export const AddMemberWrapper = styled.div`
    display: flex;
    padding: 0 10px;
    margin-bottom: 8px;
    justify-content: center;
`;

export const EmailInput = styled(Input)`
    width: 300px;
`;

export const ErrorMessage = styled.span`
    color: red;
    font-size: smaller;
    font-weight: regular;
    margin-top: -6px;
    margin-bottom: 2px;
    margin-left: 10px;
`;
