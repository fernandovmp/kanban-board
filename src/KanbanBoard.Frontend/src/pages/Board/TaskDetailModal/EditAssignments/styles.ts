import styled from 'styled-components';
import {
    DefaultButton,
    Input,
    MemberCard,
    MemberItem,
    Overlay,
    PrimaryButton,
} from '../../../../components';

export const AssignmentsOverlay = styled(Overlay)`
    top: 0;
    right: 0;
    margin: 10px;
`;

export const EmailInput = styled(Input)`
    width: 300px;
    align-self: center;
    margin-bottom: 6px;
`;

export const ButtonsWrapper = styled.div`
    margin-top: 8px;
    display: flex;
    gap: 10px;
    justify-content: center;
`;

export const CancelButton = styled(DefaultButton)`
    background-color: #ddd;
    width: 120px;
    font-size: small;
    justify-content: center;
`;

export const SaveButton = styled(PrimaryButton)`
    width: 120px;
    font-size: small;
    justify-content: center;
`;

export const AssignmentMemberItem = styled(MemberItem)`
    &:hover {
        cursor: pointer;
    }
`;

export const AssignmentMemberCard = styled(MemberCard)`
    max-width: 85%;
    min-height: 24px;
`;

export const AssignedIcon = styled.img`
    margin-left: 12px;
`;
