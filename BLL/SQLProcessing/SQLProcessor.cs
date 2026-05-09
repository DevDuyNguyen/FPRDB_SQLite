using BLL.DomainObject;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Services;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class SQLProcessor
    {
        private RecursiveDescentParser parser;
        private UpdatePlanner updatePlanner;
        private Preprocessor preProcessor;
        private QueryPlanner queryPlanner;
        private Lexer lexer;
        private ConstraintService constraintService;

        public SQLProcessor(RecursiveDescentParser parser, UpdatePlanner updatePlanner, Preprocessor preProcessor, QueryPlanner queryPlanner, Lexer lexer, ConstraintService constraintService)
        {
            this.parser = parser;
            this.updatePlanner = updatePlanner;
            this.preProcessor = preProcessor;
            this.queryPlanner = queryPlanner;
            this.lexer = lexer;
            this.constraintService = constraintService;
        }

        public bool executeDataDefinition(string sql)
        {
            this.parser.parse(sql);
            Object data = this.parser.create();
            if(data is FPRDBSchema)
            {
                FPRDBSchema createSchemaData = (FPRDBSchema)data;
                try
                {
                    if (this.preProcessor.checkSemanticCreateSchema(createSchemaData))
                    {
                        this.updatePlanner.executeCreateSchema(createSchemaData);
                        return true;
                    }
                    else
                        return false;
                }
                catch (SemanticException ex)
                {
                    throw ex;
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    throw new Exception("Something went woring");
                }
            }
            else
            {
                FPRDBRelation createRelationData = (FPRDBRelation)data;
                try
                {
                    if (this.preProcessor.checkSemanticCreateRelation(createRelationData))
                    {
                        this.updatePlanner.executeCreateRelation(createRelationData);
                        return true;
                    }
                    else
                        return false;
                }
                catch (SemanticException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    throw new Exception("Something went woring");
                }
            }
        }
        public int executeUpdate(string sql)
        {
            this.parser.parse(sql);
            Object data = this.parser.updateCommand();
            if(data is InsertData)
            {
                InsertData idata = (InsertData)data;
                if (this.preProcessor.checkSemanticInsert(idata) && this.constraintService.checkIfInsertTupleViolateReferentialConstraint(idata))
                {
                        
                    return this.updatePlanner.executeInsert(idata);
                }
            }
            else if(data is DeleteData)
            {
                DeleteData dData = (DeleteData)data;
                if (this.preProcessor.checkSemanticDelete(dData) && this.constraintService.checkIfDeleteTupleViolateReferentialConstraint(dData))
                {
                    return this.updatePlanner.executeDelete(dData);
                }
            }
            else if(data is DropRelationData)
            {
                DropRelationData DropData = (DropRelationData)data;
                if (this.preProcessor.checkSemanticDropRelation(DropData) && this.constraintService.checkIfDropRelationViolateReferentialConstraint(DropData))
                {
                    this.updatePlanner.executeDropRelation(DropData.relation);
                    return 0;
                }
            }
            else if (data is DropSchemaData)
            {
                DropSchemaData DropData = (DropSchemaData)data;
                if (this.preProcessor.checkSemanticDropSchema(DropData))
                {
                    this.updatePlanner.executeDropSchema(DropData.schema);
                    return 0;
                }
            }
            else //if(data is ModifyData)
            {
                ModifyData mData = (ModifyData)data;
                if (this.preProcessor.checkSemanticModify(mData) && this.constraintService.checkIfUpdatingTupleViolateReferentialConstraint(mData))
                {
                    return this.updatePlanner.executeModify(mData);
                }
            }
            return 0;
        }
        public Plan createQueryPlan(string sql)
        {
            this.parser.parse(sql);
            QueryData data = this.parser.query(true);

            if(this.preProcessor.checkSemanticQuery(data))
                return this.queryPlanner.createPlan(data);

            return null;

        }
        public float calculateProbabilisticInterpretationForRelationOnFuzzySetsExpression(string expression)
        {
            this.parser.parse(expression);
            RelationOnFuzzySetExpressionData data = this.parser.relationOnFuzzySetsExpression();

            if (this.preProcessor.checkSemanticRelationOnFuzzySetExpression(data))
            {
                return this.queryPlanner.calculateProbabilisticInterpretationForRelationOnFuzzySetsExpression(data);
            }
            return -1;
        }
        public TheoryCheckSelectPlan calculateProbabilisticInterpretationForSelectionExpressionOnSpecifiedTuples(string expression)
        {
            this.parser.parse(expression);
            SelectionExpressionOnSpecifiedTuplesData data = this.parser.selectionExpressionOnSpecifiedTuples();
            if (this.preProcessor.checkSemanticCalculateProbabilisticInterpretationForSelectionExpreesionOnSpecifiedTuples(data)){
                return this.queryPlanner.createPlanForCalculatingProbabilisticInterpretationForSelectionOnSpeficifiedTuple(data);
            }
            return null;
        }

    }
}
