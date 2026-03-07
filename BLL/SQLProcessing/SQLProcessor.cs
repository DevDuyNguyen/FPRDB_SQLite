using BLL.DomainObject;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class SQLProcessor
    {
        private RecursiveDescentParser parser;
        private UpdatePlanner updatePlanner;
        private Preprocessor preProcessor;

        public SQLProcessor(RecursiveDescentParser parser, UpdatePlanner updatePlanner, Preprocessor preProcessor)
        {
            this.parser = parser;
            this.updatePlanner = updatePlanner;
            this.preProcessor = preProcessor;
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
    }
}
