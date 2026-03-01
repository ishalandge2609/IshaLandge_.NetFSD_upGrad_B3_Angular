
import { analyzeMarks } from "./marks.js";

const studentMarks = [65, 70, 80, 55, 60];

const analysis = analyzeMarks(studentMarks);

console.log(`
 Student Marks Report
-----------------------
Total Marks : ${analysis.total}
Average     : ${analysis.average.toFixed(2)}
Result      : ${analysis.result}
`);