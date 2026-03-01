// marks.js

export const analyzeMarks = (marksArray) => {

    const total = marksArray.reduce((sum, mark) => sum + mark, 0);

    const average = total / marksArray.length;

    const result = average >= 40 ? "PASS" : "FAIL";

    return {
        total,
        average,
        result
    };
};