import styled from 'styled-components';
import { Card, EditableContent, Input } from '../../../components';

export const ModalCard = styled(Card)`
    display: flex;
    background: #f0f0f0;
    padding: 24px;
    margin: 30vh;
    height: 80%;
    width: 100%;
    position: relative;
`;

export const CloseButton = styled.img`
    position: absolute;
    top: 16px;
    right: 16px;
    &:hover {
        cursor: pointer;
    }
`;

export const TaskSummary = styled.h2`
    margin: 0;
    height: 30px;
`;

interface ITaskTagProps {
    color: string;
}

export const SummaryAndDescriptionSection = styled.section`
    display: flex;
    flex-direction: column;
    justify-content: stretch;
    margin-right: 40px;
    flex: 3;
`;

export const TaskTag = styled.div<ITaskTagProps>`
    border-radius: 8px;
    background-color: ${(props) => props.color};
    height: 16px;
    width: 50px;
    margin: 4px 0;
`;

export const DescriptionWrapper = styled.div`
    display: flex;
    flex-direction: column;
    margin-top: 20px;
`;

export const SectionTitle = styled.h3`
    margin: 0;
`;

export const DescriptionCard = styled.div`
    margin-top: 12px;
    width: 100%;
    padding: 12px;
    background: white;
    border-radius: 8px;
    min-height: 100px;
    font-size: 14px;
`;

export const EditableSummary = styled(EditableContent)`
    ${Input} {
        width: 100%;
        height: 30px;
    }
`;
