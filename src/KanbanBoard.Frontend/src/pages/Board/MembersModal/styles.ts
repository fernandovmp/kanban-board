import styled from 'styled-components';
import { Input } from '../../../components';

export const Header = styled.div`
    margin: 0;
    display: flex;
    flex-direction: column;
`;

export const IconButton = styled.img`
    &:hover {
        cursor: pointer;
    }
`;

export const CloseButton = styled(IconButton)`
    align-self: flex-end;
    margin-bottom: 10px;
`;

export const AddMemberWrapper = styled.div`
    display: flex;
    padding: 0 10px;
    margin-bottom: 8px;
    justify-content: center;
`;

export const MembersList = styled.ul`
    list-style: none;
    display: flex;
    flex-direction: column;
    align-items: center;
    padding-left: 10px;
    margin: 0;
    gap: 10px;
    max-width: 330px;
    height: 400px;
    overflow: auto;
`;

export const MemberItem = styled.li`
    display: inline-flex;
    width: 100%;
`;

export const MemberCard = styled.div`
    border-radius: 6px;
    background: #f0f0f0;
    font-size: small;
    padding: 4px;
    flex: 1;
    overflow: hidden;
    text-overflow: clip;
`;

export const EmailInput = styled(Input)`
    width: 300px;
`;
