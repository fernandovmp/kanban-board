import styled from 'styled-components';

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
