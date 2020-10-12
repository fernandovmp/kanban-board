import styled from 'styled-components';
import { DefaultButton } from '../../../../components';

export const SidePanelWrapper = styled.section`
    margin-top: 30px;
    display: flex;
    flex-direction: column;
    justify-content: stretch;
    flex: 1;
`;

export const SectionTitle = styled.h3`
    margin: 0;
`;

export const Button = styled(DefaultButton)`
    background: #dddddd;
    font-size: small;
    height: 30px;
    margin-top: 8px;
`;

export const AssignmentSectionTitle = styled(SectionTitle)`
    display: flex;
    align-items: center;
    margin-top: 8px;
`;

export const AssignedMemberName = styled.p`
    margin: 2px 0;
    font-size: small;
`;
