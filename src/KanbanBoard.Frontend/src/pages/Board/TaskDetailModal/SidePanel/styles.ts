import styled from 'styled-components';
import { DefaultButton, Input } from '../../../../components';

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

export const TagColorInput = styled(Input)`
    margin-top: 12px;
`;

export const TagColors = styled.ul`
    list-style: none;
    display: flex;
    flex-wrap: wrap;
    gap: 6px;
    padding: 0;
`;

interface IColorProps {
    color: string;
}

export const Color = styled.li<IColorProps>`
    width: 30px;
    height: 30px;
    background-color: ${(props) => props.color};
    border-radius: 15px;
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
